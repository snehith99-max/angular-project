import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTagcontactComponent } from './crm-trn-tagcontact.component';

describe('CrmTrnTagcontactComponent', () => {
  let component: CrmTrnTagcontactComponent;
  let fixture: ComponentFixture<CrmTrnTagcontactComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTagcontactComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTagcontactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
