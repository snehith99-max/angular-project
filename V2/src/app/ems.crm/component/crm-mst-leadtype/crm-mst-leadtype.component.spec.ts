import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmMstLeadtypeComponent } from './crm-mst-leadtype.component';

describe('CrmMstLeadtypeComponent', () => {
  let component: CrmMstLeadtypeComponent;
  let fixture: ComponentFixture<CrmMstLeadtypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmMstLeadtypeComponent]
    });
    fixture = TestBed.createComponent(CrmMstLeadtypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
