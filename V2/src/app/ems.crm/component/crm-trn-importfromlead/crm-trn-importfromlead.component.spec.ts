import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnImportfromleadComponent } from './crm-trn-importfromlead.component';

describe('CrmTrnImportfromleadComponent', () => {
  let component: CrmTrnImportfromleadComponent;
  let fixture: ComponentFixture<CrmTrnImportfromleadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnImportfromleadComponent]
    });
    fixture = TestBed.createComponent(CrmTrnImportfromleadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
