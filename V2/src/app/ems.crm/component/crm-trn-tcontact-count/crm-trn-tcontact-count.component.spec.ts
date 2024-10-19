import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTcontactCountComponent } from './crm-trn-tcontact-count.component';

describe('CrmTrnTcontactCountComponent', () => {
  let component: CrmTrnTcontactCountComponent;
  let fixture: ComponentFixture<CrmTrnTcontactCountComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTcontactCountComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTcontactCountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
