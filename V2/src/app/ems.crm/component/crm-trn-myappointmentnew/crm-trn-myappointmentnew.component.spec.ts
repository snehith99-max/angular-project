import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentnewComponent } from './crm-trn-myappointmentnew.component';

describe('CrmTrnMyappointmentnewComponent', () => {
  let component: CrmTrnMyappointmentnewComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentnewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentnewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentnewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
