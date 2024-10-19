import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecallerTeamComponent } from './crm-trn-telecaller-team.component';

describe('CrmTrnTelecallerTeamComponent', () => {
  let component: CrmTrnTelecallerTeamComponent;
  let fixture: ComponentFixture<CrmTrnTelecallerTeamComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecallerTeamComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecallerTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
