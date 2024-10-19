import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactCorporateeditComponent } from './crm-trn-contact-corporateedit.component';

describe('CrmTrnContactCorporateeditComponent', () => {
  let component: CrmTrnContactCorporateeditComponent;
  let fixture: ComponentFixture<CrmTrnContactCorporateeditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactCorporateeditComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactCorporateeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
