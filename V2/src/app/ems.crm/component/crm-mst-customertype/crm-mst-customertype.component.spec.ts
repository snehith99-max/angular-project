import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmMstCustomertypeComponent } from './crm-mst-customertype.component';

describe('CrmMstCustomertypeComponent', () => {
  let component: CrmMstCustomertypeComponent;
  let fixture: ComponentFixture<CrmMstCustomertypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmMstCustomertypeComponent]
    });
    fixture = TestBed.createComponent(CrmMstCustomertypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
