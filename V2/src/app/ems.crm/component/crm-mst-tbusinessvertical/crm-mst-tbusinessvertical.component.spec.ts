import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmMstTbusinessverticalComponent } from './crm-mst-tbusinessvertical.component';

describe('CrmMstTbusinessverticalComponent', () => {
  let component: CrmMstTbusinessverticalComponent;
  let fixture: ComponentFixture<CrmMstTbusinessverticalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmMstTbusinessverticalComponent]
    });
    fixture = TestBed.createComponent(CrmMstTbusinessverticalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
